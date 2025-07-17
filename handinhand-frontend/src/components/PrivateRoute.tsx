import { Navigate } from 'react-router-dom';
import { getJwt } from '../context/AuthContext';
import type { JSX } from 'react';

export default function PrivateRoute({ children }: { children: JSX.Element }) {
  return getJwt() ? children : <Navigate to="/login" replace />;
}
