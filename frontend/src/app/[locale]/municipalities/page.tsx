'use client';
import { useEffect, useState } from "react";
import { FeatureCollection } from 'geojson';
import MunicipalitiesMapWrapper from "./MunicipalitiesMapWrapper";

export default function Municipalities() {
    const [geojsonData, setGeojsonData] = useState<FeatureCollection | null>(null);

  useEffect(() => {
    fetch('/data/municipalities.geojson')
      .then(res => res.json())
      .then(data => setGeojsonData(data));
  }, []);

  if (!geojsonData) return <div>Ładowanie geojson...</div>;

  return <MunicipalitiesMapWrapper geojsonData={geojsonData} />;
}
