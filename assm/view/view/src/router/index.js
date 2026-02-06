import { createRouter, createWebHistory } from "vue-router";


import Menu from "../components/Menu.vue";



const routes = [
  
  {
    path: "/menu",
    name: "menu",
    component: Menu
  },
  
  
];

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior() {
    return { top: 0 };
  }
});

export default router;