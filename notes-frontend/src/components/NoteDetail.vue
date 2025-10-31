<template>
  <div class="flex items-center justify-center min-h-[300px] py-8 px-4 sm:px-6 lg:px-8 bg-gray-50 relative">
    <div class="absolute top-6 left-1/2 -translate-x-1/2 flex items-center gap-2 z-10">
      <!-- Info/Document Icon -->
      <svg class="w-7 h-7 text-blue-500" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" d="M7 7h10M7 11h10M7 15h6M5 3h14a2 2 0 012 2v14a2 2 0 01-2 2H5a2 2 0 01-2-2V5a2 2 0 012-2z"/>
      </svg>
      <h1 class="text-2xl font-bold text-gray-900">Note Details</h1>
    </div>
    <button
      @click="$emit('close')"
      class="absolute top-6 right-8 text-gray-400 hover:text-red-600 transition z-10"
      aria-label="Close"
      type="button"
    >
      <svg class="w-6 h-6" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
      </svg>
    </button>
    <div class="w-full max-w-md relative mt-20">
      <div class="rounded-2xl p-6 bg-white shadow-2xl">
        <div class="flex flex-col items-center mb-6">
          <h2 class="text-2xl sm:text-3xl font-extrabold text-gray-900 text-center mb-1 truncate max-w-xs">
            {{ note.title }}
          </h2>
        </div>
        <div class="mb-6">
          <div class="text-gray-700 text-base whitespace-pre-line break-words text-center" v-if="note.content">
            {{ note.content }}
          </div>
          <div class="text-gray-400 italic text-center" v-else>No content provided.</div>
        </div>
        <div class="flex flex-col sm:flex-row sm:justify-between gap-2 border-t pt-4 text-sm text-gray-500">
          <div class="flex items-center gap-2 justify-center">
            <span class="font-medium text-gray-600">Created:</span>
            <span>{{ formatDate(note.createdAt) }}</span>
          </div>
          <div class="flex items-center gap-2 justify-center" v-if="note.createdBy">
            <span class="font-medium text-gray-600">By:</span>
            <span>{{ note.createdBy }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import formatDateKhmer from '@/composables/formatDate';
import { defineComponent } from 'vue';

export default defineComponent({
  props: {
    note: {
      type: Object,
      required: true
    }
  },
  setup() {
    return {
      formatDate: formatDateKhmer
    };
  }
});
</script>
