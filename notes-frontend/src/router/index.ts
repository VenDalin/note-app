import LoginView from '@/views/LoginView.vue';
import NoteCreate from '@/components/NoteCreate.vue';
import NoteDetail from '@/components/NoteDetail.vue';
import NoteEdit from '@/components/NoteEdit.vue';
import NotesView from '@/views/NotesView.vue';
import RegisterView from '@/views/RegisterView.vue';
import Welcome from '@/views/Welcome.vue';
import { createRouter, createWebHistory } from 'vue-router';

const routes = [
  { path: '/', name: 'Home', component: NotesView },
  { path: '/welcome', name: 'Welcome', component: Welcome }, // Add Welcome route
  { path: '/notes/create', name: 'Create', component: NoteCreate },
  { path: '/notes/:id/edit', name: 'Edit', component: NoteEdit, props: true },
  { path: '/notes/:id', name: 'Detail', component: NoteDetail, props: true },
  { path: '/login', name: 'Login', component: LoginView },
  { path: '/register', name: 'Register', component: RegisterView }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach((to, from, next) => {
  const publicPages = ['/welcome' ,'/login', '/register', ]; // Make welcome public
  const authRequired = !publicPages.includes(to.path);
  const token = localStorage.getItem('token');

  if (authRequired && !token) {
    return next('/welcome');
  }
  next();
});

export default router;
