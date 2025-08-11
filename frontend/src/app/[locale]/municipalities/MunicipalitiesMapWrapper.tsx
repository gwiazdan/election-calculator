import dynamic from 'next/dynamic';

const MunicipalitiesMap = dynamic(() => import('./MunicipalitiesMap'), {
  ssr: false,
});

export default function Page(props: any) {
  return <MunicipalitiesMap {...props} />;
}
