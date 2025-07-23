/// <reference types="cypress" />
import { uuid } from '../support/commands';

describe('求助模块', () => {
  const cred = {
    userName: `requestUser_${uuid()}`,
    email:    `request_${uuid()}@test.com`,
    password: 'Abc123!'
  };

  before(() => {
    cy.register(cred);
  });

  beforeEach(() => {
    cy.login(cred);
  });

  it('可以创建求助请求', () => {
    cy.intercept('POST', '**/api/helprequests').as('createHelp');

    cy.visit('/requests/new');

    cy.get('input[placeholder="标题"]').type('E2E 求助标题');
    cy.get('textarea[placeholder="描述"]').type('E2E 求助内容');
    cy.contains('button', '提交').click();

    cy.wait('@createHelp').its('response.statusCode')
      .should('be.oneOf', [200, 201]);

    cy.url().should('include', '/requests');
    cy.contains('E2E 求助标题');
  });

  it('可以删除自己的求助', () => {
    // 假设上一用例创建的标题仍可复用
    cy.visit('/my/requests');

    cy.contains('E2E 求助标题')
        .closest('div')
        .within(() => {
        cy.contains('删除').click();
        });

    cy.on('window:confirm', () => true);
    cy.contains('E2E 求助标题').should('not.exist');
  });
});
