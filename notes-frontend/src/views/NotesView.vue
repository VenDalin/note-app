<template>
  <div class="p-4 max-w-7xl mx-auto mt-4">
    <div class="flex flex-col md:flex-row md:justify-between items-center gap-3 mb-6">
      <input v-model="q" @keyup.enter="search" placeholder="Search notes..."
        class="border border-gray-300 p-3 rounded-lg w-full md:w-1/2 shadow-sm focus:ring-2 transition duration-200" />
      <div class="flex items-center gap-4">
        <label class="sr-only" for="sort-notes">Sort</label>

        <div class="flex items-center gap-2">
          <div class="relative">
            <input type="date" v-model="startDate"
              class="pl-10 pr-3 py-2 rounded-lg border border-gray-300 shadow-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition text-gray-700 bg-white min-w-[140px]"
              placeholder="Start date" />
          </div>
          <span class="text-gray-500 font-medium">to</span>
          <div class="relative">
            <input type="date" v-model="endDate"
              class="pl-10 pr-3 py-2 rounded-lg border border-gray-300 shadow-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition text-gray-700 bg-white min-w-[140px]"
              placeholder="End date" />
          </div>




          <button @click="refreshDateFilter"
            class="inline-flex items-center gap-2 bg-gradient-to-r from-blue-600 to-blue-700 hover:from-blue-700 hover:to-blue-800 text-white px-6 py-3 rounded-lg shadow-lg transition duration-200 font-medium focus:outline-none focus:ring-2 focus:ring-blue-500">
           Refresh
          </button>



        </div>

        <button @click="showCreateModal = true"
          class="inline-flex items-center gap-2 bg-green-700  text-white px-6 py-3 rounded-lg shadow-lg transition duration-200 font-medium ">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" d="M12 4v16m8-8H4" />
          </svg>
          Add
        </button>
      </div>
    </div>

    <div v-if="loading" class="text-gray-500 text-center py-12 text-lg">Loading...</div>

    <div v-else-if="userNotes.length === 0" class="text-gray-500 text-center py-12 text-lg">
      No notes found. Create your first note!
    </div>

    <div v-else class="bg-white shadow-lg rounded-lg overflow-hidden">
      <div class="overflow-x-auto">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th class="px-6 py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">#</th>
              <th class="px-6 py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Title</th>
              <th class="px-6 py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Content</th>
              <th class="px-6 py-4 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">Created</th>
              <th class="px-6 py-4 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">Updated</th>
              <th class="px-6 py-4 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-for="(note, index) in userNotes" :key="note.id" class="hover:bg-gray-50 transition duration-150">
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="text-sm font-medium text-gray-900">{{ index + 1 }}</div>
              </td>
              <td class="px-6 py-4">
                <div class="text-sm text-gray-700 line-clamp-2">{{ note.title }}</div>
              </td>
              <td class="px-6 py-4">
                <div class="text-sm text-gray-700 line-clamp-2">{{ note.content }}</div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="text-sm text-gray-500 text-center">{{ formatDate(note.createdAt) }}</div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="text-sm text-gray-500 text-center">{{ note.updatedAt ? formatDate(note.updatedAt) : '-' }}
                </div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-center">
                <button @click="viewNote(note)"
                  class="text-blue-600 hover:text-blue-900 mr-4 transition duration-150">View</button>
                <button @click="editNote(note)"
                  class="text-gray-600 hover:text-gray-900 mr-4 transition duration-150">Edit</button>
                <button @click="openDeleteConfirm(note)"
                  class="text-red-600 hover:text-red-900 transition duration-150">Delete</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="userNotes.length > 0" class="mt-8 flex flex-col items-end">
      <div class="text-gray-600 text-sm">Showing {{ userNotes.length }} of {{ total }} notes</div>
      <Pagination :page="page" :totalPages="Math.ceil(total / pageSize)" @update:page="onPageChange" />
    </div>

    <!-- Create Note Modal -->
    <div v-if="showCreateModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div ref="createModalRef" class="bg-white rounded-lg shadow-xl max-w-md w-full mx-4 p-6">
        <NoteCreate @close="closeCreateModal" @created="onNoteCreated" />
      </div>
    </div>

    <!-- View Note Modal -->
    <div v-if="showViewModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div ref="viewModalRef" class="bg-white rounded-lg shadow-xl max-w-lg w-full mx-4 p-6">
        <NoteDetail :note="selectedNote" @close="showViewModal = false" />
      </div>
    </div>

    <!-- Edit Note Modal -->
    <div v-if="showEditModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div ref="editModalRef" class="bg-white rounded-lg shadow-xl max-w-md w-full mx-4 p-6">
        <NoteEdit :note="selectedNote" @close="closeEditModal" @updated="onNoteUpdated" />
      </div>
    </div>

    <ConfirmDelete v-if="showDeleteConfirm" @cancel="showDeleteConfirm = false" @confirm="handleDelete" />
  </div>
</template>

<script lang="ts">
import ConfirmDelete from '@/components/ConfirmDelete.vue';
import NoteCreate from '@/components/NoteCreate.vue';
import NoteDetail from '@/components/NoteDetail.vue';
import NoteEdit from '@/components/NoteEdit.vue';
import Pagination from '@/components/Pagination.vue';
import formatDateKhmer from '@/composables/formatDate';
import { useClickOutside } from '@/composables/useClickOutside';
import api from '@/services/api';
import { useNotesStore } from '@/stores/notes';
import { computed, defineComponent, onMounted, ref, watch } from 'vue';

interface Note {
  id: number;
  title: string;
  content: string;
  createdAt: string;
  updatedAt?: string;
  userId?: number;
  createdBy?: string;
}

export default defineComponent({
  components: { Pagination, NoteCreate, NoteDetail, NoteEdit, ConfirmDelete },
  setup() {
    const notes = ref<Note[]>([]);
    const total = ref(0);
    const page = ref(1);
    const pageSize = ref(10);
    const loading = ref(false);
    const q = ref('');
    const sort = ref('created_desc');
    const startDate = ref('');
    const endDate = ref('');
    const notesStore = useNotesStore();
    const showCreateModal = ref(false);
    const showViewModal = ref(false);
    const showEditModal = ref(false);
    const selectedNote = ref<Note | null>(null);
    const showDeleteConfirm = ref(false);
    const noteToDelete = ref<Note | null>(null);


    const createModalRef = ref<HTMLElement | null>(null);
    const viewModalRef = ref<HTMLElement | null>(null);
    const editModalRef = ref<HTMLElement | null>(null);

    const closeOnClickOutside = ref(false);

    if (closeOnClickOutside.value) {
      useClickOutside(createModalRef, () => { showCreateModal.value = false; });
      useClickOutside(viewModalRef, () => { showViewModal.value = false; });
      useClickOutside(editModalRef, () => { showEditModal.value = false; });
    }

    const userNotes = computed(() => {
      return notes.value.filter(note => note.userId === notesStore.userId);
    });

    const fetch = async () => {
      loading.value = true;
      try {
        const res = await api.get('/api/notes', {
          params: {
            searchTerm: q.value,
            sort: sort.value,
            page: page.value,
            pageSize: pageSize.value,
            startDate: startDate.value || undefined,
            endDate: endDate.value || undefined
          }
        });
        notes.value = res.data.items;
        total.value = res.data.totalCount || res.data.total;
      } finally {
        loading.value = false;
      }
    };

    onMounted(fetch);

    watch([q, sort, startDate, endDate], () => {
      page.value = 1;
      fetch();
    });

    const onPageChange = (newPage: number) => {
      page.value = newPage;
      fetch();
    };

    const viewNote = (note: Note) => {
      selectedNote.value = note;
      showViewModal.value = true;
    };

    const editNote = (note: Note) => {
      selectedNote.value = note;
      showEditModal.value = true;
    };

    const closeCreateModal = () => {
      showCreateModal.value = false;
    };

    const closeEditModal = () => {
      showEditModal.value = false;
      selectedNote.value = null;
    };

    const onNoteCreated = () => {
      showCreateModal.value = false;
      fetch();
    };

    const onNoteUpdated = () => {
      showEditModal.value = false;
      selectedNote.value = null;
      fetch();
    };

    const openDeleteConfirm = (note: Note) => {
      noteToDelete.value = note;
      showDeleteConfirm.value = true;
    };

    const handleDelete = async () => {
      if (!noteToDelete.value) return;
      await api.delete(`/api/notes/${noteToDelete.value.id}`);
      showDeleteConfirm.value = false;
      noteToDelete.value = null;
      fetch();
    };

    const refreshDateFilter = () => {
      q.value = '';
      startDate.value = '';
      endDate.value = '';
      page.value = 1;
      fetch();
    };

    return {
      notes,
      total,
      loading,
      q, sort, page, pageSize,
      startDate, endDate,
      userNotes,
      showCreateModal,
      showViewModal,
      showEditModal,
      selectedNote,
      showDeleteConfirm,
      openDeleteConfirm,
      handleDelete,
      search: () => { page.value = 1; fetch(); },
      onPageChange,
      viewNote,
      editNote,
      closeCreateModal,
      closeEditModal,
      onNoteCreated,
      onNoteUpdated,
      remove: async (id: number) => {
        if (!confirm('Delete this note?')) return;
        await api.delete(`/api/notes/${id}`);
        fetch();
      },
      formatDate: formatDateKhmer,
      createModalRef,
      viewModalRef,
      editModalRef,
      refreshDateFilter,
    };
  }
});
</script>

<style scoped>
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>