import { uuid } from '../support/commands';   // 与 skills/requests 共用

describe('认证流程', () => {
  // 把随机账号提到外层变量，两个用例复用
  const user = {
    userName: `authUser_${uuid()}`,
    email:    `auth_${uuid()}@test.com`,
    password: 'Abc123!'
  };

  it('用户可以注册并自动登录', () => {
    cy.wrap(user).as('freshUser');        // 保存别名

    cy.visit('/register');
    cy.get('input[placeholder="用户名"]').type(user.userName);
    cy.get('input[placeholder="邮箱"]').type(user.email);
    cy.get('input[placeholder="密码"]').type(user.password);
    cy.contains('button', '注册并登录').click();

    cy.url().should('eq', `${Cypress.config().baseUrl}/`);
    cy.window().its('localStorage.jwt').should('exist');
  });

  it('用户可以登录', () => {
    // 此时数据库里肯定已有 user
    cy.visit('/login');
    cy.get('input[placeholder="用户名"]').type(user.userName);
    cy.get('input[placeholder="密码"]').type(user.password);
    cy.contains('button', '登录').click();

    cy.url().should('eq', `${Cypress.config().baseUrl}/`);
    cy.window().its('localStorage.jwt').should('exist');
  });
});
