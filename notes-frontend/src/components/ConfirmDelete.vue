<template>
  <div class="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
    <div class="bg-white rounded-xl shadow-xl p-8 max-w-sm w-full mx-4 flex flex-col items-center">
      <svg class="w-14 h-14 text-red-500 mb-4" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12"/>
      </svg>
      <h3 class="text-xl font-bold text-gray-800 mb-2 text-center">Delete Note?</h3>
      <p class="text-gray-600 mb-6 text-center">Are you sure you want to delete this note? This action cannot be undone.</p>
      <div class="flex gap-4 w-full">
        <button
          @click="$emit('cancel')"
          class="flex-1 py-2 rounded-lg bg-gray-200 text-gray-700 font-semibold hover:bg-gray-300 transition"
        >
          Cancel
        </button>
        <button
          @click="$emit('confirm')"
          class="flex-1 py-2 rounded-lg bg-red-600 text-white font-semibold hover:bg-red-700 transition"
        >
          Delete
        </button>
      </div>
    </div>
  </div>
</template>