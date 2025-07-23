import { uuid } from '../support/commands';

describe('个人资料修改（含密码）', () => {
  const cred = {
    userName: `profile_${uuid()}`,
    email:    `profile_${uuid()}@test.com`,
    password: 'Abc123!'        // 初始密码
  };

  // 用随机新密码防止冲突
  const newPwd  = `New${uuid()}!`;
  const newName = `Cypress_${uuid()}`;
  const newEmail= `cypress_${uuid()}@test.com`;

  before(() => cy.register(cred));

  it('用户可以修改用户名、邮箱和密码并保存', () => {
    cy.login(cred);

    cy.intercept('PUT', '**/api/users/me').as('updateProfile');

    cy.visit('/profile');
    cy.contains('编辑').click();

    /* -- 用户名 & 邮箱 -- */
    cy.get('input[placeholder="用户名"], input[name="userName"]')
      .clear().type(newName);

    cy.get('input[placeholder="邮箱"], input[name="email"]')
      .clear().type(newEmail);

    /* -- 密码 -- */
    cy.get('input[placeholder="旧密码"], input[name="oldPassword"]')
      .type(cred.password);

    cy.get('input[placeholder="新密码"], input[name="newPassword"]')
      .type(newPwd);

    cy.contains('button', '保存').click();

    cy.wait('@updateProfile')
      .its('response.statusCode')
      .should('be.oneOf', [200, 204]);

    /* --- 重新登录验证新密码 --- */
    cy.contains('退出').click();            // 若页面无此按钮，请用你实际的退出流程

    cy.visit('/login');
    cy.get('input[placeholder="用户名"]').type(newName);
    cy.get('input[placeholder="密码"]').type(newPwd);
    cy.contains('button', '登录').click();

    cy.url().should('eq', `${Cypress.config().baseUrl}/`);
    cy.window().its('localStorage.jwt').should('exist');
  });
});
