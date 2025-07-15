import axios from 'axios';

const baseURL = import.meta.env.VITE_API_URL || 'http://localhost:5129';

export const api = axios.create({ baseURL });

api.interceptors.request.use(cfg => {
  const token = localStorage.getItem('jwt');
  if (token) cfg.headers.Authorization = `Bearer ${token}`;
  return cfg;
});
