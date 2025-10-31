<template>
  <div class="flex items-center justify-center py-8 px-6 sm:px-6 lg:px-8">
    <div class="w-full max-w-lg relative"> 

      <div class="rounded-2xl p-2">
       
        <div class="text-center mb-6">
          <div class="flex justify-center">
            <span class="inline-flex items-center justify-center w-14 h-14 rounded-full bg-blue-100">
              <svg class="w-8 h-8 text-blue-600" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" d="M12 20h9" />
                <path stroke-linecap="round" stroke-linejoin="round"
                  d="M16.5 3.5a2.121 2.121 0 113 3L7 19.5 3 21l1.5-4L16.5 3.5z" />
              </svg>
            </span>
          </div>
          <h2 class="text-3xl font-extrabold text-gray-900 mb-1">Create New Note</h2>
        </div>
        <form @submit.prevent="submit" class="space-y-5">
          <div>
            <label for="title" class="block text-sm font-semibold text-gray-700 mb-1">
              Title <span class="text-red-500">*</span>
            </label>
            <input id="title" v-model="title" required
              class="block w-full px-4 py-3 border border-gray-300 rounded-lg shadow-sm placeholder-gray-400 transition"
              placeholder="Enter note title" />
          </div>
          <div>
            <label for="content" class="block text-sm font-semibold text-gray-700 mb-1">Content</label>
            <textarea id="content" v-model="content" rows="5"
              class="block w-full px-4 py-3 border border-gray-300 rounded-lg shadow-sm placeholder-gray-400 transition resize-none"
              placeholder="Enter note content (optional)"></textarea>
          </div>
          <div v-if="error" class="text-red-600 text-sm bg-red-50 border border-red-200 rounded-md p-3">
            {{ error }}
          </div>
          <div class="flex gap-3 pt-2">
            <button type="button" @click="$emit('close')"
              class="flex-1 py-3 rounded-lg border border-gray-300 bg-gray-100 text-gray-700 font-semibold shadow hover:bg-gray-200 transition">
              Cancel
            </button>
            <button type="submit"
              class="flex-1 py-3 rounded-lg bg-gradient-to-r from-blue-600 to-blue-700 text-white font-semibold shadow hover:from-blue-700 hover:to-blue-800 transition">
              Save Note
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import api from '@/services/api';
import { defineComponent, ref } from 'vue';

export default defineComponent({
  emits: ['close', 'created'],
  setup(_, { emit }) {
    const title = ref('');
    const content = ref('');
    const error = ref('');

    const submit = async () => {
      error.value = '';
      if (!title.value.trim()) {
        error.value = 'Title is required.';
        return;
      }
      try {
        const noteData = { Title: title.value, Content: content.value };
        await api.post('/api/notes', noteData);
        emit('created');
      } catch (err: any) {
        if (err.response?.status === 401) {
          error.value = 'You are not authorized. Please log in again.';
        } else {
          error.value = 'Failed to create note. Please try again.';
        }
      }
    };

    return { title, content, error, submit };
  }
});
</script>
