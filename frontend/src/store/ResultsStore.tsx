import { createStore } from 'zustand/vanilla';
import { useStore } from 'zustand';
import { ResultColors, Results, getMunicipalityResults } from './api/ApiFetcher';
import { Party } from "@/enums/Party";
import { calculate_results, calculate_gradient } from '@/wasm/ElectionCalculator';
import { colors } from '@/enums/Colors';

export type ResultsState = {
	origResults: Results[];
	isLoading: boolean;
	error: string | null;
	fetchOriginalResults: () => Promise<void>;
	calculateResults: () => Promise<Results[]>;
	calculateGradient: (results: Results[]) => Promise<ResultColors[]>;
};

const refResults: Record<Party, number> = {
	[Party.LEW]: 0.041366,
	[Party.KKP]: 0.061814,
	[Party.PL2050]: 0.048735,
	[Party.KONFEDERACJA]: 0.144354,
	[Party.PIS]: 0.287945,
	[Party.KO]: 0.306608,
	[Party.RAZEM]: 0.047525,
	[Party.MN]: 0.000971,
	[Party.PSL]: 0.022882,
	[Party.OTHERS]: 0.037799
};

const newResults: Record<Party, number> = {
    [Party.LEW]: 4.1366,
    [Party.KKP]: 6.1814,
    [Party.PL2050]: 4.8735,
    [Party.KONFEDERACJA]: 22.4354,
    [Party.PIS]: 20.7945,
    [Party.KO]: 30.6608,
    [Party.RAZEM]: 4.7525,
    [Party.MN]: 0.0971,
    [Party.PSL]: 2.2882,
    [Party.OTHERS]: 3.7799
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

	return await calculate_results(input, ref, newR);
	},

	calculateGradient: async (results: Results[]) => {
		const state = resultsStore.getState();
		const input: Results[] = state.origResults.map(result => ({
			id: result.id,
			name: result.name,
			totalVotes: result.totalVotes,
			votes: Object.fromEntries(
				Object.entries(result.votes).map(([key, value]) => [key, value ?? 0])
			)
		}));

		return await calculate_gradient(input, colors);
	}
}));

export const useResultsStore = () => useStore(resultsStore);
