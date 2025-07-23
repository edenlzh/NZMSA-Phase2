/// <reference types="cypress" />
import { uuid } from '../support/commands';

describe('技能模块', () => {
  const cred = {
    userName: `skillUser_${uuid()}`,
    email:    `skill_${uuid()}@test.com`,
    password: 'Abc123!'
  };

  before(() => {
    cy.register(cred);          // 接口级注册
  });

  beforeEach(() => {
    cy.login(cred);             // 登录写入 JWT
  });

  it('可以创建技能', () => {
    cy.intercept('POST', '**/api/skills').as('createSkill');

    cy.visit('/skills/new');
    cy.get('input[placeholder="标题"]').type('Cypress 技能');
    cy.get('textarea[placeholder="描述"]').type('E2E 测试创建的技能');
    cy.contains('button', '提交').click();

    cy.wait('@createSkill').its('response.statusCode').should('be.oneOf', [200, 201]);
    cy.contains('Cypress 技能');    // 创建成功的验证
  });

  it('可以删除自己的技能', () => {
    cy.visit('/my/skills');         // ★ 关键：进入真正的管理页

    // 找到标题所在卡片并在其作用域内点击“删除”
    cy.contains('Cypress 技能')
      .closest('div')               // 先找到最近的 div 卡片容器；如需更精确可换成 .closest('.skill-card')
      .within(() => {
        cy.contains('删除').click();
      });

    cy.on('window:confirm', () => true);    // 接受确认框

    // 验证已移除
    cy.contains('Cypress 技能').should('not.exist');
  });
});
