import type { Metadata } from 'next';
import { Geist, Geist_Mono } from 'next/font/google';
import './globals.css';
import Navbar from '../components/main/SideNavbar';
import TopNavbar from '../components/main/TopNavbar';
import { NextIntlClientProvider } from 'next-intl';
import { getMessages } from 'next-intl/server';

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
    const messages = await getMessages();

    return (
        <NextIntlClientProvider messages={messages}>
            <html lang="en">
                <head>
                    <link rel="icon" type="image/svg+xml" href="/appLogo.svg" />
                </head>
                <body className={`${geistSans.variable} ${geistMono.variable} antialiased`}>
                    <TopNavbar />
                    <Navbar />
                    {children}
                </body>
            </html>
        </NextIntlClientProvider>
    );
}
