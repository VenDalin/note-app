<template>
    <nav class="flex items-center justify-between bg-blue-600 px-6 py-3 shadow text-white">
        <div class="text-xl font-bold tracking-wide">Note App</div>
        <button v-if="isLoggedIn" @click="logout"
            class="bg-white text-blue-600 px-4 py-2 rounded hover:bg-blue-100 font-semibold transition">
            Logout
        </button>
        <button v-if="!isLoggedIn" @click="register"
            class="bg-white text-blue-600 px-4 py-2 rounded hover:bg-blue-100 font-semibold transition">
            Register
        </button>
    </nav>
</template>

<script setup lang="ts">
import { useNotesStore } from '@/stores/notes';
import { computed } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const notesStore = useNotesStore();

const isLoggedIn = computed(() => notesStore.userId !== null && notesStore.userId !== undefined);

function register() {
    router.push('/register');
}

function logout() {
    notesStore.clearUser();
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('username');
    router.push('/welcome');
}
</script>