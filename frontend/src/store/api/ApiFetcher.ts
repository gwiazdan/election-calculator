import { Party } from "@/enums/Party";

export type Results = {
	id: number;
	name: string;
	totalVotes: number;
	votes: Votes;
}

export type ResultColors = {
	id: number;
	name: string;
	colors: Record<Party, string>;
}


const apiPort = '5225';

export type Votes = Partial<Record<Party, number | null>>;

export const pingAPI = async (): Promise<boolean> => {
	const response = await fetch(`http://localhost:${apiPort}/api/ping`); // Zmieniono na HTTP dla testu
	return response.ok;
}

export const getMunicipalityResults = async (): Promise<Results[]> => {
	const response = await fetch(`http://localhost:${apiPort}/api/municipality/load`); // Zmieniono na HTTP dla testu
	if (!response.ok) {
		throw new Error("Failed to fetch municipality results");
	}
	const data: Results[] = await response.json();
	const filled = data.map(result => {
		const votes = Object.fromEntries(
			Object.entries(result.votes).map(([k, v]) => [k, v ?? 0])
		) as Votes;

		const votesSum = Object.values(votes).reduce((a, b) => (a ?? 0) + (b ?? 0), 0) ?? 0;
		const missing = result.totalVotes - votesSum;

		if (missing > 0) {
			votes[Party.OTHERS] = (votes[Party.OTHERS] ?? 0) + missing;
		}

		return {
			...result,
			votes
		};
	});

	return filled;
}
