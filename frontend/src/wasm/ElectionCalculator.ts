import { ResultColors, Results } from '@/store/api/ApiFetcher';
import init, { calculate_gradient_colors, process_results } from 'election-calculator-wasm';


export async function calculate_results(
  results: Results[],
  refResults: Record<string, number>,
  x: Record<string, number>
): Promise<Results[]> {
  await init();
  return process_results(
    JSON.stringify(results),
    JSON.stringify(refResults),
    JSON.stringify(x)
  );
}

export async function calculate_gradient(
  results: Results[],
  colors: Record<string, string>
): Promise<ResultColors[]> {
  await init();
  return calculate_gradient_colors(
    JSON.stringify(results),
    JSON.stringify(colors)
  );
}