import moment from 'moment-timezone';

const khmerMonths: string[] = [
  'មករា', 'កុម្ភៈ', 'មីនា', 'មេសា', 'ឧសភា', 'មិថុនា',
  'កក្កដា', 'សីហា', 'កញ្ញា', 'តុលា', 'វិច្ឆិកា', 'ធ្នូ'
];

export default function formatDateKhmer(date: string | Date | undefined): string {
  if (!date) return '';
  
  const momentDate = moment(date);
  const day: number = momentDate.date();
  const monthIndex: number = momentDate.month(); // 0-11
  const year: number = momentDate.year();
  
  return `${day} ${khmerMonths[monthIndex]} ${year}`;
}