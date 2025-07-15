import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

const resources = {
  zh: {
    translation: {
      home: '主页',
      skills: '技能库',
      requests: '求助',
      login: '登录',
      logout: '退出',
      postSkill: '发布技能',
      postRequest: '发布求助',
      title: '标题',
      description: '描述',
      categoryOptional: '类别（可选）',
      submit: '提交',
      userName: '用户名',
      password: '密码',
      email: '邮箱',
      noAccount: '没有账号？',
      register: '注册',
      registerFailed: '注册失败',
      registerAndLogin: '注册并登录',
      emailLabel: '邮箱',
      phoneLabel: '电话',
      copyright: '© {{year}} HandInHand',
      homeIntro: '欢迎来到 HandInHand！这是一个帮助社区成员分享技能和寻求帮助的平台。',
      homeDesc: '在这里，你可以分享你的技能并寻求社区帮助。',
    },
  },
  en: {
    translation: {
      home: 'Home',
      skills: 'Skills',
      requests: 'Help',
      login: 'Login',
      logout: 'Logout',
      postSkill: 'Post Skill',
      postRequest: 'Ask Help',
      title: 'Title',
      description: 'Description',
      categoryOptional: 'Category (Optional)',
      submit: 'Submit',
      userName: 'User Name',
      password: 'Password',
      email: 'Email',
      noAccount: 'No Account?',
      register: 'Register',
      registerFailed: 'Register Failed',
      registerAndLogin: 'Register and Login',
      emailLabel: 'Email',
      phoneLabel: 'Phone',
      copyright: '© {{year}} HandInHand',
      homeIntro: 'Welcome to HandInHand! This is a platform for community members to share skills and seek help.',
      homeDesc: 'Here, you can share your skills and seek help from the community.',
    },
  },
};

i18n.use(initReactI18next).init({
  resources,
  lng: localStorage.getItem('lang') || 'zh',
  fallbackLng: 'zh',
  interpolation: { escapeValue: false },
});

export default i18n;
