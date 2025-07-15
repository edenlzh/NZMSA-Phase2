import { api } from './client';

export interface AuthResponse { token: string }

export const register = (data: {userName: string; email: string; password: string;}) =>
  api.post<AuthResponse>('/api/auth/register', data).then(r => r.data);

export const login = (data: {userName: string; password: string;}) =>
  api.post<AuthResponse>('/api/auth/login', data).then(r => r.data);
