'use client';
import React from 'react';

const TopNavbar: React.FC = () => {
    return (
        <nav className="hidden md:flex w-full h-[80px] bg-neutral-800 items-center justify-between px-8 shadow-md z-[1000]">
            <div className="font-bold text-2xl tracking-wide">Election Calculator</div>
            <div className="flex gap-6"></div>
        </nav>
    );
};

export default TopNavbar;
