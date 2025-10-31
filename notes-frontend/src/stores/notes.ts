import { defineStore } from 'pinia';


export const useNotesStore = defineStore('notes', {
  state: () => ({
    userId: null as number | null,
    userName: '' as string,
  }),
  actions: {
    setUserId(id: number | null) {
      this.userId = id;
    },
    setUserName(name: string) {
      this.userName = name;
    },
    clearUser() {
      this.userId = null;
      this.userName = '';
    }
  },
  getters: {
    getUserId: (state) => state.userId,
    getUserName: (state) => state.userName,
  },
  persist: true, 
});
