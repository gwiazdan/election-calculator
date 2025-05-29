import { redirect } from 'next/navigation';

export default function Home() {
    redirect('/input-panel');
    return null;
}
