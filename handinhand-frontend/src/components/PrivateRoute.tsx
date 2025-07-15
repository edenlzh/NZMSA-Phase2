import { type ReactElement } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

/**
 * 受保护路由包装：如未登录则跳到 /login，并把原路径存在 state.from
 */
export default function PrivateRoute({ children }: { children: ReactElement }) {
  const { jwt } = useAuth();
  const location = useLocation();
  return jwt ? (
    children
  ) : (
    <Navigate to="/login" state={{ from: location }} replace />
  );
}
