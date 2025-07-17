import React from 'react';
import ReactDOM from 'react-dom/client';
import { RouterProvider } from 'react-router-dom';
import { router } from './routes';
import AuthProvider from './context/AuthContext';
import { store } from './app/store';
import { Provider } from 'react-redux';
import './index.css';
import './i18n';
import ThemeProvider from './context/ThemeContext';
import LangProvider from './context/LangContext';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const qc = new QueryClient();

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <Provider store={store}>
      <AuthProvider>
        <ThemeProvider>
          <LangProvider>
            <QueryClientProvider client={qc}>
              <RouterProvider router={router} />
            </QueryClientProvider>
          </LangProvider>
        </ThemeProvider>
      </AuthProvider>
    </Provider>
  </React.StrictMode>,
);
