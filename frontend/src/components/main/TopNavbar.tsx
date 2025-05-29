'use client';
import React from 'react';

const TopNavbar: React.FC = () => {
    return (
        <nav className="hidden md:flex w-full h-[60px] bg-secondary items-center justify-between px-8 shadow-md fixed top-0 left-0 z-[1000] bg-primary">
            <div className="font-bold text-xl tracking-wide">Election Calculator</div>
            <div className="flex gap-6"></div>
        </nav>
    );
};

export default TopNavbar;
