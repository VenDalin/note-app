// src/services/api.ts
import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5077', 
});

api.interceptors.request.use(
  (config) => {
   
    const token = localStorage.getItem('token'); 

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {

    if (error.response?.status === 401) {
      console.error('Unauthorized! Redirecting to login...');
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default api;