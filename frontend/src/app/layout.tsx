import type { Metadata } from 'next';
import { Geist, Geist_Mono } from 'next/font/google';
import './globals.css';
import Navbar from '../components/main/SideNavbar';
import TopNavbar from '../components/main/TopNavbar';
import { NextIntlClientProvider } from 'next-intl';
import { getLocale } from 'next-intl/server';

const geistSans = Geist({
    variable: '--font-geist-sans',
    subsets: ['latin'],
});

const geistMono = Geist_Mono({
    variable: '--font-geist-mono',
    subsets: ['latin'],
});

export const metadata: Metadata = {
    title: 'Election Calculator',
    description:
        'Election Calculator for calculating seats and vote distribution in polish elections',
};

export default async function RootLayout({ children }: Readonly<{ children: React.ReactNode }>) {
    const locale = await getLocale();

    return (
        <NextIntlClientProvider>
            <html lang={locale}>
                <head>
                    <link rel="icon" type="image/svg+xml" href="/appLogo.svg" />
                </head>
                <body className={`${geistSans.variable} ${geistMono.variable} antialiased`}>
					<Navbar />
					<div className='md:ml-[120px] ml-[100px]'>
						<TopNavbar />
						{children}
					</div>
                </body>
            </html>
        </NextIntlClientProvider>
    );
}
