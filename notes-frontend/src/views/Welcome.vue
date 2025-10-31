<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-100 via-white">
    <div class="flex flex-col items-center justify-center min-h-[80vh]">
      <div class="bg-white rounded-2xl shadow-xl px-10 py-12 flex flex-col items-center max-w-lg w-full">
        <svg class="w-16 h-16 text-blue-500 mb-6" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" d="M17 8h2a2 2 0 012 2v8a2 2 0 01-2 2H5a2 2 0 01-2-2v-8a2 2 0 012-2h2M12 12v6m0 0l-3-3m3 3l3-3M12 6v6" />
        </svg>
        <h1 class="text-4xl font-extrabold mb-4 text-blue-700 text-center">Welcome to Note App</h1>
        <p class="text-lg text-gray-600 mb-8 text-center">
          Your personal space to create, edit, and manage notes efficiently.<br>
        </p>
        <router-link
          to="/login"
          class="bg-blue-600 text-white px-8 py-3 rounded-full shadow hover:bg-blue-700 transition font-semibold text-lg"
        >
          Go to Notes
        </router-link>
      </div>
    </div>
  </div>
</template>
