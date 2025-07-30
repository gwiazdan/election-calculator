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
  [Party.NL]: 0.0423,
  [Party.KKP]: 0.0634,
  [Party.TD]: 0.0499,
  [Party.KONFEDERACJA]: 0.1481,
  [Party.PIS]: 0.2954,
  [Party.KO]: 0.3136,
  [Party.RAZEM]: 0.0486,
  [Party.MN]: 0.00005,
  [Party.OTHERS]: 0.03815
};

const newResults: Record<Party, number> = {
	   [Party.NL]: 4.23,
	   [Party.KKP]: 6.34,
	   [Party.TD]: 4.99,
	   [Party.KONFEDERACJA]: 19.81,
	   [Party.PIS]: 24.54,
	   [Party.KO]: 31.36,
	   [Party.RAZEM]: 4.86,
	   [Party.MN]: 0.005,
	   [Party.OTHERS]: 3.815
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
