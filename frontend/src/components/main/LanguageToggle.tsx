'use client';

import { useLocale } from "next-intl";
import { usePathname, useRouter } from "next/navigation";

import Poland from "@/navbar_icons/poland.svg";
import USA from "@/navbar_icons/usa.svg";


export default function LanguageToggle() {
	const router = useRouter();
	const pathname = usePathname();
	const locale = useLocale();

  const toggleLanguage = () => {
    const newLocale = locale === "pl" ? "en" : "pl";
    const pathWithoutLocale = pathname.replace(/^\/(pl|en)/, '');
    router.push(`/${newLocale}${pathWithoutLocale}`);
  };

  return (
    <button className="w-12 h-12 shadow rounded-full inline-block hover:cursor-pointer"
      onClick={toggleLanguage}
    >
	  {locale === "pl" ? <USA/> : <Poland/>}
    </button>
  );
}
