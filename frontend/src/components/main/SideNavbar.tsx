'use client';
import React from 'react';

const navItems = [
    { label: 'Panel sterowania', href: '/input-panel' },
    { label: 'Gminy', href: '/municipalities' },
    { label: 'Powiaty', href: '/counties' },
    { label: 'Województwa', href: '/voivodeships' },
    { label: 'Sejm', href: '/sejm'},
    { label: 'Senat', href: '/senate' },
    { label: 'Sejmiki', href: '/sejmiks' },
    { label: 'Eurowybory', href: '/euro-elections' },
    { label: 'O mnie', href: '/about' },
    { label: 'FAQ', href: '/faq' },
];

export default function Navbar() {
    return (
        <nav className="fixed left-0 top-0 md:top-[60px] h-screen w-[300px] bg-neutral-900 flex flex-col p-8 shadow-lg z-[100]">
            <ul className="list-none p-0 m-0 flex-1">
                {navItems.map((item) => (
                    <li key={item.href} className="mb-5">
                        <a href={item.href} className="block text-white no-underline text-base px-4 py-2 rounded transition-colors duration-200 hover:bg-neutral-800">
                            {item.label}
                        </a>
                    </li>
                ))}
            </ul>
        </nav>
    );
}