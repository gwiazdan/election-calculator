import { Party } from "@/enums/Party";

export type Results = {
	id: number;
	name: string;
	totalVotes: number;
	votes: Votes;
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
	return data;
}
