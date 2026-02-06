// src/config/app.config.js

export const APP_CONFIG = {
  // Domain backend (ASP.NET)
  BACKEND_BASE_URL: import.meta.env.VITE_BACKEND_URL || "https://localhost:7045",

  // Domain frontend (Vue)
  FRONTEND_BASE_URL: import.meta.env.VITE_FRONTEND_URL || "http://localhost:5173",

  // Route
  
  SEARCH_PATH: "/Product/Search"
};