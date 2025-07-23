/// <reference types="cypress" />

declare global {
  namespace Cypress {
    interface Chainable {
      /**
       * 通过后端接口注册并自动写入 JWT
       */
      register(user: {
        userName: string;
        email: string;
        password: string;
      }): Chainable<void>;

      /**
       * 通过后端接口登录并写入 JWT
       */
      login(user: {
        userName: string;
        password: string;
      }): Chainable<void>;

      /**
       * 更新当前登录用户的基本信息
       */
      updateProfile(body: {
        displayName?: string;
        phone?: string;
      }): Chainable<void>;

      /**
       * 发表评论
       */ 
      postComment(helpRequestId: number, content: string): Chainable<void>;
    }
  }
}

Cypress.Commands.add('register', (user) => {
  cy.request('POST', `${Cypress.env('apiUrl')}/api/auth/register`, user)
    .its('body')
    .then((res: { token: string }) => {
      window.localStorage.setItem('jwt', res.token);
    });
});

Cypress.Commands.add('login', (user) => {
  cy.request('POST', `${Cypress.env('apiUrl')}/api/auth/login`, user)
    .its('body')
    .then((res: { token: string }) => {
      window.localStorage.setItem('jwt', res.token);
    });
});

/**
 * 更新当前登录用户的基本信息
 * body 形如：{ displayName: '新名字', phone: '123...' }
 */
Cypress.Commands.add('updateProfile', (body) => {
  cy.request({
    method: 'PUT',
    url: `${Cypress.env('apiUrl')}/api/users/me`,
    body,
    headers: { Authorization: `Bearer ${window.localStorage.getItem('jwt')}` },
  });
});

/** 发表评论 */
Cypress.Commands.add('postComment', (helpRequestId: number, content: string) => {
  cy.request({
    method: 'POST',
    url: `${Cypress.env('apiUrl')}/api/helprequests/${helpRequestId}/comments`,
    body: { content },
    headers: { Authorization: `Bearer ${window.localStorage.getItem('jwt')}` },
  });
});

export function uuid(): string {
  // 简单随机串，也可用 nanoid
  return Date.now().toString(36) + Math.random().toString(36).substring(2, 5);
}


export {};   // ← 关键！使文件成为模块，允许 declare global
