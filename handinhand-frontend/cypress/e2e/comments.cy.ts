import { uuid } from '../support/commands';

describe('帖子评论功能', () => {
  const cred = {
    userName: `comment_${uuid()}`,
    email:    `comment_${uuid()}@test.com`,
    password: 'Abc123!'
  };

  let helpId:  number;
  let skillId: number;

  before(() => {
    // 1) 注册并写入 JWT
    cy.register(cred);

    // 2) 取 token，再依次创建求助 & 技能
    cy.window().then((win) => {
      const token = win.localStorage.getItem('jwt');

      // 创建求助
      cy.request({
        method: 'POST',
        url: `${Cypress.env('apiUrl')}/api/helprequests`,   // ← 若路由不同请改
        body: {
          title:       '评论测试求助',
          description: 'E2E 求助内容'
        },
        headers: { Authorization: `Bearer ${token}` },
      }).its('body').then((res) => { helpId = res.id; });

      // 创建技能
      cy.request({
        method: 'POST',
        url: `${Cypress.env('apiUrl')}/api/skills`,         // ← 若路由不同请改
        body: {
          title:       '评论测试技能',
          description: 'E2E 技能内容'
        },
        headers: { Authorization: `Bearer ${token}` },
      }).its('body').then((res) => { skillId = res.id; });
    });
  });

  beforeEach(() => cy.login(cred));

  it('可以在求助帖中发表评论', () => {
    cy.visit(`/requests/${helpId}`);              // 求助详情页
    cy.get('input[placeholder="留言"]').type('求助帖第一条 Cypress 评论');
    cy.contains('button', '发送').click();
    cy.contains('求助帖第一条 Cypress 评论').should('be.visible');
  });

  it('可以在技能帖中发表评论', () => {
    cy.visit(`/skills/${skillId}`);               // 技能详情页
    cy.get('input[placeholder="留言"]').type('技能帖第一条 Cypress 评论');
    cy.contains('button', '发送').click();
    cy.contains('技能帖第一条 Cypress 评论').should('be.visible');
  });
});
