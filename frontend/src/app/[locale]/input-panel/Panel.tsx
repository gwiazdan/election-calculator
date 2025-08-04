import { Party } from "@/enums/Party";
import { PartySlider } from "./PartySlider";
import { useTranslations } from 'next-intl';

export const Panel = () => {
	const t = useTranslations('InputPanel');

	return (
		<form className='flex flex-col items-center my-2'>
			<PartySlider party={Party.NL} name={t('party.nl')} />
			<PartySlider party={Party.KKP} name={t('party.kkp')} />
			<PartySlider party={Party.PSL} name={t('party.psl')} />
			<PartySlider party={Party.PL2050} name={t('party.pl2050')} />
			<PartySlider party={Party.PIS} name={t('party.pis')} />
			<PartySlider party={Party.KO} name={t('party.ko')} />
			<PartySlider party={Party.RAZEM} name={t('party.razem')} />
			<PartySlider party={Party.KONFEDERACJA} name={t('party.konfederacja')} />
		</form>
	);
};