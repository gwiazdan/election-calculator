import { Results } from '@/store/api/ApiFetcher';
import init, { process_results } from 'election-calculator-wasm';


export default async function calculate(
  results: Results[],
  refResults: Record<string, number>,
  x: Record<string, number>
): Promise<any> {
  await init();
  return process_results(
    JSON.stringify(results),
    JSON.stringify(refResults),
    JSON.stringify(x)
  );
}
