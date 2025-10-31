<template>
  <div class="flex items-center justify-center min-h-screen bg-gradient-to-br from-blue-200 via-white">
    <div v-if="isLoading" class="fixed inset-0 z-50 flex flex-col items-center justify-center bg-white bg-opacity-80">
      <Vue3Lottie :animationData="loadingAnim" :loop="true" :autoplay="true" :height="400" :width="400" />
      <div class="mt-4 text-blue-600 font-semibold text-lg">Registering...</div>
    </div>

    <div class="w-full max-w-md p-10 bg-white rounded-3xl shadow-2xl">
      <div class="flex flex-col items-center mb-8">
        <svg class="w-14 h-14 text-blue-500 mb-3" fill="none" stroke="currentColor" stroke-width="2"
          viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round"
            d="M12 11c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm0 2c-2.67 0-8 1.34-8 4v3h16v-3c0-2.66-5.33-4-8-4z" />
        </svg>
        <h2 class="text-3xl font-extrabold text-gray-800 mb-1">Create Account</h2>
      </div>
      <form @submit.prevent="register" class="space-y-6">
        <div>
          <label class="block text-gray-700 mb-1 font-medium" for="username">Username</label>
          <input id="username" v-model="username" placeholder="Choose a username"
            class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400 transition"
            required autocomplete="username" />
        </div>
        <div>
          <label class="block text-gray-700 mb-1 font-medium" for="password">Password</label>
          <input id="password" v-model="password" type="password" placeholder="Create a password"
            class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400 transition"
            required autocomplete="new-password" />
        </div>
        <button type="submit" :disabled="loading"
          class="w-full py-2 bg-gradient-to-r from-blue-500 to-blue-400 text-white font-semibold rounded-lg shadow hover:from-blue-600 hover:to-blue-500 transition disabled:opacity-50">
          {{ loading ? 'Registering...' : 'Register' }}
        </button>
      </form>
      <div v-if="error" class="mt-5 text-red-600 text-center font-medium">{{ error }}</div>
      <div class="mt-4 text-center">
        <span class="text-gray-600">Already have an account? </span>
        <router-link to="/login" class="text-blue-500 hover:text-blue-600 font-medium">Login</router-link>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import loadingAnim from '@/assets/LoadingLogin.json';
import api from '@/services/api';
import { useNotesStore } from '@/stores/notes';
import { defineComponent, ref } from 'vue';
import { useRouter } from 'vue-router';
import { Vue3Lottie } from "vue3-lottie";

export default defineComponent({
  components: { Vue3Lottie },
  setup() {
    const router = useRouter();
    const notesStore = useNotesStore(); 
    const username = ref('');
    const password = ref('');
    const error = ref('');
    const loading = ref(false);
    const isLoading = ref(false);

    const register = async () => {
      error.value = '';
      loading.value = true;
      isLoading.value = true;
      try {
        const res = await api.post('/api/auth/register', {
          username: username.value.trim(),
          password: password.value
        });
        localStorage.setItem('token', res.data.token);
        localStorage.setItem('userId', res.data.userId);
        localStorage.setItem('username', res.data.username);
        notesStore.setUserId(res.data.userId);
        notesStore.setUserName(res.data.username);
        setTimeout(() => {
          router.push('/');
        }, 1500);
      } catch (err: any) {
        error.value = err.response?.data || 'Registration failed. Please try again.';
        isLoading.value = false;
      } finally {
        loading.value = false;
      }
    };

    return { username, password, register, error, loading, isLoading, loadingAnim };
  }
});
</script>