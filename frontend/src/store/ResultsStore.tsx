import { createStore } from 'zustand/vanilla';
import { useStore } from 'zustand';
import { Results, getMunicipalityResults } from './api/ApiFetcher';

export type ResultsState = {
  origResults: Results[];
  calcResults: Results[] | null;
  isLoading: boolean;
  error: string | null;
  fetchOriginalResults: () => Promise<void>;
};

export const resultsStore = createStore<ResultsState>((set) => ({
  origResults: [],
  isLoading: false,
  error: null,
  calcResults: null,


  fetchOriginalResults: async () => {
    set({ isLoading: true, error: null });

    try {
      const data = await getMunicipalityResults();
      set({ origResults: data, isLoading: false });
    } catch (e) {
      if (e instanceof Error) {
        set({ error: e.message, isLoading: false });
      } else {
        set({ error: 'An unknown error occurred', isLoading: false });
      }
    }
  },

  tempSetResults: () => { // Temporary solution to the lack of calculation logic
  	set((state) => ({ calcResults: state.origResults }));
  },

  calculateResults: () => set(() => ({
    // Todo: Implement calculation logic here
  })),
}));

export const useResultsStore = () => useStore(resultsStore);
