'use client';
import { useTranslations } from 'next-intl';
import React from 'react';
import MunicipalitiesIcon from '@/navbar_icons/municipalities.svg';
import CountiesIcon from '@/navbar_icons/counties.svg';
import VoivodeshipIcon from '@/navbar_icons/voivodeship.svg';
import SlidersIcon from '@/navbar_icons/sliders.svg';
import InformationIcon from '@/navbar_icons/info-icon.svg';
import SenateIcon from '@/navbar_icons/senate.svg';
import SejmIcon from '@/navbar_icons/sejm.svg';
import SejmikIcon from '@/navbar_icons/sejmik.svg';
import EuroparlamentIcon from '@/navbar_icons/eu.svg';
import '@/components/main/navbar.css';

const iconClasses = 'inline-block w-8 h-8 mr-2';

export default function Navbar() {
    const t = useTranslations('SideNavbar');
    const navItems = [
        {
            label: t('inputPanel'),
            href: '/input-panel',
            icon: <SlidersIcon className={iconClasses} />,
        },
        {
            label: t('municipalities'),
            href: '/municipalities',
            icon: <MunicipalitiesIcon className={iconClasses} />,
        },
        { label: t('counties'), href: '/counties', icon: <CountiesIcon className={iconClasses} /> },
        {
            label: t('voivodeships'),
            href: '/voivodeships',
            icon: <VoivodeshipIcon className={iconClasses} />,
        },
        { label: t('sejm'), href: '/sejm', icon: <SejmIcon className={iconClasses} /> },
        { label: t('senate'), href: '/senate', icon: <SenateIcon className={iconClasses} /> },
        { label: t('sejmiks'), href: '/sejmiks', icon: <SejmikIcon className={iconClasses} /> },
        {
            label: t('euroElections'),
            href: '/euro-elections',
            icon: <EuroparlamentIcon className={iconClasses} />,
        },
        { label: t('info'), href: '/info', icon: <InformationIcon className={iconClasses} /> },
    ];

    return (
        <nav className="fixed left-0 top-0 h-screen md:w-[140px] w-[100px] bg-neutral-900 flex flex-col p-6 shadow-lg z-[100]">
            <ul className="list-none p-0 m-0 flex-1">
                {navItems.map((item) => (
                    <li key={item.href} className="mb-4">
                        <a
                            href={item.href}
                            className="flex flex-col items-center justify-center gap-2 no-underline text-base px-2 py-1 rounded transition-colors duration-200 hover:bg-neutral-800"
                        >
                            <span className="block">{item.icon}</span>
                            <span className="hidden lg:block text-center lg:text-base sb-label">
                                {item.label}
                            </span>
                        </a>
                    </li>
                ))}
            </ul>
        </nav>
    );
}
