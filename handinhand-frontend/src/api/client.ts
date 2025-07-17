import axios from 'axios';
import { getJwt } from '../context/AuthContext';

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE || 'http://localhost:5129',
});

api.interceptors.request.use(cfg => {
  const token = getJwt();
  if (token) cfg.headers.Authorization = `Bearer ${token}`;
  return cfg;
});
