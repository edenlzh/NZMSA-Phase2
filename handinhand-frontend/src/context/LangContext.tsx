import { createContext, useContext, useState, type ReactNode } from 'react';
import i18n from '../i18n';

type Lang = 'zh' | 'en';
interface Ctx { lang: Lang; toggle: () => void }
const LangContext = createContext<Ctx>({ lang: 'zh', toggle() {} });
export const useLang = () => useContext(LangContext);

export default function LangProvider({ children }: { children: ReactNode }) {
  const [lang, setLang] = useState<'zh' | 'en'>(
    () => (i18n.language as 'zh' | 'en') ?? 'zh',
  );

  const toggle = () => {
    const next = lang === 'zh' ? 'en' : 'zh';
    i18n.changeLanguage(next);
    localStorage.setItem('lang', next);
    setLang(next);                 // 关键：更新 state，闭包永远最新
  };

  return (
    <LangContext.Provider value={{ lang, toggle }}>
      {children}
    </LangContext.Provider>
  );
}
