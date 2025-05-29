'use client';
import { useTranslations } from 'next-intl';
import React from 'react';



export default function Navbar() {
	const t = useTranslations('SideNavbar');
	const navItems = [
        { label: t('inputPanel'), href: '/input-panel' },
        { label: t('municipalities'), href: '/municipalities' },
        { label: t('counties'), href: '/counties' },
        { label: t('voivodeships'), href: '/voivodeships' },
        { label: t('sejm'), href: '/sejm' },
        { label: t('senate'), href: '/senate' },
        { label: t('sejmiks'), href: '/sejmiks' },
        { label: t('euroElections'), href: '/euro-elections' },
        { label: t('about'), href: '/about' },
        { label: t('faq'), href: '/faq' },
    ];

    return (
        <nav className="fixed left-0 top-0 md:top-[60px] h-screen w-[300px] bg-neutral-900 flex flex-col p-8 shadow-lg z-[100]">
            <ul className="list-none p-0 m-0 flex-1">
                {navItems.map((item) => (
                    <li key={item.href} className="mb-5">
                        <a
                            href={item.href}
                            className="block text-white no-underline text-base px-4 py-2 rounded transition-colors duration-200 hover:bg-neutral-800"
                        >
                            {item.label}
                        </a>
                    </li>
                ))}
            </ul>
        </nav>
    );
}
