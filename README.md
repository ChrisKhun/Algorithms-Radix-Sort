# 📊 Radix Sort Performance Analysis

### Team Members
- **Christopher Khun**
- **Alexander Wilson**
- **David Nub**

---

## 🧩 Overview
This project implements **Radix Sort** and compares it to **Quick Sort**, **Merge Sort**, and **Heap Sort** to determine whether Radix Sort is truly the more efficient algorithm across various dataset types and sizes. Radix Sort is a **non-comparative** sorting algorithm that processes numbers digit by digit. Under certain conditions (especially with large datasets of uniformly sized numeric keys), it can achieve near **linear-time performance**, outperforming traditional comparison-based algorithms.

---

## 🎯 Objectives
- Implement and benchmark **Radix Sort**, **Quick Sort**, **Merge Sort**, and **Heap Sort**  
- Measure runtime across multiple dataset types and input sizes  
- Analyze how data characteristics impact efficiency  
- Present findings visually and collaboratively as a team

---

## ⚙️ Dataset Types
- Random integers  
- Nearly sorted data  
- Reverse-sorted data  
- Wide and narrow numeric ranges  
- Small to large dataset sizes

**Example CSV snippet**  
    27,71,5,30,43,76,49,9,90,21,99,35,7,25,86,40,17,68,83

---

## 🧠 Methodology
- **Counting Sort** used as a stable subroutine within Radix Sort  
- Uniform timing harness and identical machine/build settings for fairness  
- Multiple trials per dataset; report mean and variance  
- Results visualized via charts and tables

---

## 📈 Key Findings 
-
-
-
-

---

## 🧩 Technologies Used
- **Languages:** c# 
- **Tools:** GitHub, VS Code, spreadsheets or matplotlib for charts  
- **Data:** CSV inputs and timing logs

---

## 🗣️ Presentation & Teamwork
- Clear division of responsibilities (implementation, data generation, analysis)  
- Slide deck focused on visuals (charts/tables) and succinct takeaways  
- Rehearsed delivery and shared Q&A coverage

---

## 📚 References
- *Introduction to Algorithms* — Cormen, Leiserson, Rivest, and Stein  
- Classic references on Radix/Counting Sort stability and complexity  
- Course notes and benchmarking best practices

---

## 🏁 Conclusion
Radix Sort can deliver near-linear performance on **large, numeric, and uniform datasets**, but **Quick Sort** and **Merge Sort** remain strong all-around choices. The “best” algorithm depends on input size, key distribution, and practical overheads—so selecting the right tool for the data is essential.

---

**Course:** CPSC 3125 – Algorithms  
**Institution:** Columbus State University  
**Semester:** Fall 2025
