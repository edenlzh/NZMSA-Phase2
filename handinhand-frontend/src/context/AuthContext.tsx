import { createContext, useContext, useState } from 'react';
import type { ReactNode } from 'react';

interface AuthCtx {
  jwt: string | null;
  setJwt: (t: string | null) => void;
  logout: () => void;
}
const AuthContext = createContext<AuthCtx>({ jwt: null, setJwt: () => {}, logout(){} });
export const useAuth = () => useContext(AuthContext);

export default function AuthProvider({ children }: { children: ReactNode }) {
  const [jwt, setJwtState] = useState<string | null>(
    () => localStorage.getItem('jwt') ?? null,
  );
  const setJwt = (t: string | null) => {
    if (t) localStorage.setItem('jwt', t);
    else localStorage.removeItem('jwt');
    setJwtState(t);
  };
  const logout = () => setJwt(null);
  return (
    <AuthContext.Provider value={{ jwt, setJwt, logout }}>
      {children}
    </AuthContext.Provider>
  );
}
