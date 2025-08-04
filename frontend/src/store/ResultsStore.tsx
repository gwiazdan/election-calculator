import { createStore } from 'zustand/vanilla';
import { useStore } from 'zustand';
import { Results, getMunicipalityResults } from './api/ApiFetcher';
import { Party } from "@/enums/Party";
import calculate from '@/wasm/ElectionCalculator';

export type ResultsState = {
	origResults: Results[];
	isLoading: boolean;
	error: string | null;
	fetchOriginalResults: () => Promise<void>;
	calculateResults: () => Promise<Results[]>;
};

const refResults: Record<Party, number> = {
  [Party.NL]: 0.04102,
  [Party.KKP]: 0.061323,
  [Party.PL2050]: 0.048451,
  [Party.KONFEDERACJA]: 0.14381,
  [Party.PIS]: 0.288167,
  [Party.KO]: 0.308465,
  [Party.RAZEM]: 0.048022,
  [Party.MN]: 0.001198,
  [Party.PSL]: 0.023173,
  [Party.OTHERS]: 0.036371
};

const newResults: Record<Party, number> = {
	   [Party.NL]: 4.1,
	   [Party.KKP]: 6.13,
	   [Party.PL2050]: 4.84,
	   [Party.KONFEDERACJA]: 19.81,
	   [Party.PIS]: 23.88,
	   [Party.KO]: 30.84,
	   [Party.RAZEM]: 4.8,
	   [Party.MN]: 0.1198,
	     [Party.PSL]: 2.3173,
	   [Party.OTHERS]: 3.6371
};

export const resultsStore = createStore<ResultsState>((set) => ({
	origResults: [],
	isLoading: false,
	error: null,

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

	
	calculateResults: async () => {
	const state = resultsStore.getState();
	const input: Results[] = state.origResults.map(result => ({
		id: result.id,
		name: result.name,
		totalVotes: result.totalVotes,
		votes: Object.fromEntries(
		Object.entries(result.votes).map(([key, value]) => [key, value ?? 0])
		)
	}));

	const ref = Object.fromEntries(
		Object.entries(refResults).map(([k, v]) => [k.toString(), v])
	);

	const newR = Object.fromEntries(
		Object.entries(newResults).map(([k, v]) => [k.toString(), v / 100])
	);

	return await calculate(input, ref, newR);
}
	
}));

export const useResultsStore = () => useStore(resultsStore);
