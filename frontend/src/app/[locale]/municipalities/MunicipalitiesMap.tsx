'use client';
import React, {useEffect, useState} from "react";
import { FeatureCollection } from 'geojson';
import {MapContainer, GeoJSON} from "react-leaflet";
import { useResultsStore } from "@/store/ResultsStore";
import { Party } from "@/enums/Party";


const MunicipalitiesMap: React.FC<{ geojsonData: FeatureCollection }> = ({ geojsonData }) => {
  const { calculateResults, origResults, calculateGradient } = useResultsStore();
  const [activeMunicipality, setActiveMunicipality] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [colorMap, setColorMap] = useState<Record<string, string>>({});

	useEffect(() => {
	const loadAndCalculate = async () => {
		setLoading(true);
		const results = await calculateResults();
		const gradient = await calculateGradient(results);
		console.log(gradient);

		// Mapowanie id gminy na kolor
		const newColorMap: Record<string, string> = {};
		for (const result of results) {
		const id = result.id.toString();

		let maxVotes = 0;
		let winningParty: Party | null = null;

		for (const party in result.votes) {
			const votes = result.votes[party as Party] ?? 0;
			if (votes > maxVotes) {
			maxVotes = votes;
			winningParty = party as Party;
			}
		}

		const row = gradient.find(row => row.id === result.id);
		if (winningParty && row?.colors[winningParty]) {
			newColorMap[id] = row.colors[winningParty];
		} else {
			newColorMap[id] = '#ccc'; // kolor domyślny jeśli brak danych
		}
		}
		console.log('geojsonData:', geojsonData);
		setColorMap(newColorMap);
	setLoading(false);
	};

	if (origResults.length > 0) {
		loadAndCalculate();
	}
	}, [calculateGradient, calculateResults, geojsonData, origResults]);

	// Funkcja stylu dla GeoJSON — kolorujemy wg colorMap i podkreślamy zaznaczoną gminę
	const style = (feature: any) => {
		const id = feature.properties.id;
		// dopasuj do swojego GeoJSON
		return {
			fillColor: id && colorMap[id.toString()] ? colorMap[id.toString()] : '#eee',
			weight: id === activeMunicipality ? 5 : 1.2,
			color: id === activeMunicipality ? 'white' : 'black',
			fillOpacity: 1,
		};
	};
	// Obsługa kliknięcia na gminę
	const onEachFeature = (feature: any, layer: any) => {
		layer.on({
			click: () => {
			// podświetl cały feature (wielopolygon) — leafet robi to domyślnie
			setActiveMunicipality(feature.properties.id);
			},
		});
	};

	if (loading) return <div>Ładowanie mapy...</div>;

	

	return (
		<MapContainer
			center={[52, 19]}
			zoom={6.4}
			style={{ height: "900px", width: "60%"}}
			scrollWheelZoom={true}>
			<GeoJSON data={geojsonData} style={style} onEachFeature={onEachFeature} />
		</MapContainer>
	);
};
export default MunicipalitiesMap;

