import type { Metadata } from 'next';
import { Geist, Geist_Mono } from 'next/font/google';
import './globals.css';
import SideNavbar from '@/components/main/SideNavbar';
import TopNavbar from '@/components/main/TopNavbar';
import DataInitializer from '@/components/DataInitializer';
import { hasLocale, NextIntlClientProvider } from 'next-intl';
import { routing } from '@/i18n/routing';
import { notFound } from 'next/navigation';

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

export default async function RootLayout({
	children,
	params
}: {
	children: React.ReactNode;
	params: Promise<{locale: string}>;
}) {
	const {locale} = await params;
	if (!hasLocale(routing.locales, locale)) notFound();

	return (
		<NextIntlClientProvider>
			<html lang={locale}>
				<head>
					<link rel="icon" type="image/svg+xml" href="/appLogo.svg" />
				</head>
				<body className={`${geistSans.variable} ${geistMono.variable} antialiased`}>
					<DataInitializer />
					<SideNavbar />
					<div className='md:ml-[120px] ml-[100px]'>
						<TopNavbar />
						{children}
					</div>
				</body>
			</html>
		</NextIntlClientProvider>
    );
}
