<template>
  <div class="flex items-center justify-center gap-2 mt-6">
    <button
      class="px-3 py-1 rounded border bg-white hover:bg-gray-100 disabled:opacity-50 disabled:cursor-not-allowed"
      :disabled="page <= 1"
      @click="$emit('update:page', page - 1)"
    >
      Prev
    </button>
    
    <template v-for="item in visiblePages" :key="item.toString()">
      <span v-if="item === '...'" class="px-2 text-gray-500">...</span>
      <button
        v-else
        class="px-3 py-1 rounded border"
        :class="(item as number) === page ? 'bg-blue-500 text-white' : 'bg-white hover:bg-gray-100'"
        :disabled="(item as number) === page"
        @click="$emit('update:page', item as number)"
      >
        {{ item }}
      </button>
    </template>
    
    <button
      class="px-3 py-1 rounded border bg-white hover:bg-gray-100 disabled:opacity-50 disabled:cursor-not-allowed"
      :disabled="page >= totalPages"
      @click="$emit('update:page', page + 1)"
    >
      Next
    </button>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  page: number;
  totalPages: number;
}>();

const emit = defineEmits<{
  (e: 'update:page', value: number): void;
}>();

const visiblePages = computed(() => {
  const delta = 2; // Number of pages to show around the current page
  const pages: (number | string)[] = [];
  
  if (props.totalPages <= 1) return pages;
  
  // Always show first page
  pages.push(1);
  
  // Calculate range around current page
  const start = Math.max(2, props.page - delta);
  const end = Math.min(props.totalPages - 1, props.page + delta);
  
  // Add "..." if there's a gap after first page
  if (start > 2) {
    pages.push('...');
  }
  
  // Add pages in range
  for (let i = start; i <= end; i++) {
    pages.push(i);
  }
  
  // Add "..." if there's a gap before last page
  if (end < props.totalPages - 1) {
    pages.push('...');
  }
  
  // Always show last page if more than 1 page
  if (props.totalPages > 1) {
    pages.push(props.totalPages);
  }
  
  return pages;
});
</script>