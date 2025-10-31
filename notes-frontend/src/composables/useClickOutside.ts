import { onBeforeUnmount, onMounted, Ref } from 'vue';

export function useClickOutside(targetRef: Ref<HTMLElement | null>, handler: () => void) {
  const listener = (event: MouseEvent) => {
    const el = targetRef.value;
    if (!el || el.contains(event.target as Node)) return;
    handler();
  };

  onMounted(() => {
    document.addEventListener('mousedown', listener);
  });
  onBeforeUnmount(() => {
    document.removeEventListener('mousedown', listener);
  });
}