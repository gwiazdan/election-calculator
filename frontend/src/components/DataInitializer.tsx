'use client';
import { useEffect } from 'react';
import { useResultsStore } from "@/store/ResultsStore";

export default function DataInitializer() {
	const { fetchOriginalResults } = useResultsStore();

	useEffect(() => {
		const loadData = async () => {
			await fetchOriginalResults();
		};
		loadData();
	}, [fetchOriginalResults]);

	return null; // Ten komponent nie renderuje nic
}
