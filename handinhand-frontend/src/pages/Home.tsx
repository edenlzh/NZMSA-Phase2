
import HandShake from '../assets/HandShake.png';
import { useTranslation } from 'react-i18next';


export default function Home() {
  const { t } = useTranslation();
  return (
    <section className="main-content flex-1 flex flex-col justify-center responsive-container">
      <h1 className="text-2xl font-bold dark:text-yellow-200 mb-6 text-center">{t('home')}</h1>
      <p className="dark:text-yellow-100 text-center">{t('homeIntro')}</p>
      <div className="flex justify-center items-center w-full">
        <div className="w-full max-w-2xl h-56 bg-brand-dark/20 flex items-center justify-center rounded-lg overflow-hidden">
          <img
            src={HandShake}
            alt="Banner"
            className="h-full object-contain mx-auto"
          />
        </div>
      </div>
      <p className="leading-relaxed dark:text-yellow-100 text-center text-lg mt-4">
        {t('homeDesc')}
      </p>
    </section>
  );
}
